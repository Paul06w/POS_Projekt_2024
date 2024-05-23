const addBox = document.querySelector(".add-box"),
    popupBox = document.querySelector(".popup-box"),
    popupTitle = popupBox.querySelector("header p"),
    closeIcon = popupBox.querySelector("header i"),
    titleTag = popupBox.querySelector("input"),
    descTag = popupBox.querySelector("textarea"),
    addBtn = popupBox.querySelector("button");
const months = ["January", "February", "March", "April", "May", "June", "July",
    "August", "September", "October", "November", "December"];


//showNotes();
//localStorage.setItem("notes", JSON.stringify(notes));

localStorage.clear();
const notes = JSON.parse(localStorage.getItem("notes") || "[]");

localStorage.clear();
fetchNotesFromServer().then(r => showNotes());


let isUpdate = false, updateId;
addBox.addEventListener("click", () => {
    popupTitle.innerText = "Add a new Note";
    addBtn.innerText = "Add Note";
    popupBox.classList.add("show");
    document.querySelector("body").style.overflow = "hidden";
    if(window.innerWidth > 660) titleTag.focus();
});
closeIcon.addEventListener("click", () => {
    isUpdate = false;
    titleTag.value = descTag.value = "";
    popupBox.classList.remove("show");
    document.querySelector("body").style.overflow = "auto";
});
function showNotes() {
    if(!notes) return;
    document.querySelectorAll(".note").forEach(li => li.remove());
    notes.forEach((note, id) => {
        let filterDesc = note.description.replaceAll("\n", '<br/>');
        let liTag = `<li class="note">
                        <div class="details">
                            <input type="checkbox" id="checkbox-${id}" class="note-checkbox" ${note.checked ? 'checked' : ''}>
                            <!-- <label for="checkbox-${id}" class="checkbox-label">${note.title}</label>-->
                            <!--<p>${note.title}</p>-->
                            <span>${filterDesc}</span>
                        </div>
                        <div class="bottom-content">
                            <!--<span>${note.date}</span>-->
                            <span>${note.title}</span>
                            <div class="settings">
                                <i onclick="showMenu(this)" class="uil uil-ellipsis-h"></i>
                                <ul class="menu">
                                    <li onclick="updateNote(${id}, '${note.title}', '${filterDesc}')"><i class="uil uil-pen"></i>Edit</li>
                                    <li onclick="deleteNote(${id})"><i class="uil uil-trash"></i>Delete</li>
                                </ul>
                            </div>
                        </div>
                    </li>`;
        addBox.insertAdjacentHTML("afterend", liTag);
    });
}
showNotes();
function showMenu(elem) {
    elem.parentElement.classList.add("show");
    document.addEventListener("click", e => {
        if(e.target.tagName != "I" || e.target != elem) {
            elem.parentElement.classList.remove("show");
        }
    });
}
function deleteNote(noteId) {
    //console.log(noteId)
    let confirmDel = confirm("Are you sure you want to delete this note?");
    if(!confirmDel) return;
    const noteIdMongo = notes[noteId].id
    //console.log(noteIdMongo)
    notes.splice(noteId, 1);
    deleteNotesFromServer(noteIdMongo)
    localStorage.setItem("notes", JSON.stringify(notes));
    showNotes();
}
function updateNote(noteId, title, filterDesc) {
    let description = filterDesc.replaceAll('<br/>', '\r\n');
    updateId = noteId;
    isUpdate = true;
    addBox.click();
    titleTag.value = title;
    descTag.value = description;
    popupTitle.innerText = "Update a Note";
    addBtn.innerText = "Update Note";
}
addBtn.addEventListener("click", e => {
    e.preventDefault();
    let title = titleTag.value.trim(),
        description = descTag.value.trim();
    if(title || description) {
        let currentDate = new Date(),
            month = months[currentDate.getMonth()],
            day = currentDate.getDate(),
            year = currentDate.getFullYear();
        let noteInfo = {title, description, date: `${month} ${day}, ${year}`}
        if(!isUpdate) {
            notes.push(noteInfo);
        } else {
            isUpdate = false;
            notes[updateId] = noteInfo;
        }
        localStorage.setItem("notes", JSON.stringify(notes));
        showNotes();
        closeIcon.click();
    }
});




async function fetchNotesFromServer() {
    try {
        // Senden einer GET-Anfrage an den Server
        const response = await fetch('http://10.10.3.7:8080/api/notizen');

        // Überprüfen, ob die Anfrage erfolgreich war (Statuscode 200)
        if (response.ok) {
            // Konvertieren der Antwort in JSON
            const notesData = await response.json();

            // Anzeigen der Notizen im Notiz-Container
            notesData.forEach(note => {
                //const noteElement = document.createElement("p");
                //addSection(note.id, note.title, note.text, note.check)
                //noteElement.textContent = note.title;
                //notesContainer.appendChild(noteElement);
                let title = note.title;
                let description = note.text.replace(/;/g, "\n");
                let check = note.check;
                let id = note.id;
                let date = note.title;
                let currentDate = new Date(),
                    month = months[currentDate.getMonth()],
                    day = currentDate.getDate(),
                    year = currentDate.getFullYear();
                let noteInfo = {id, check, title, description, date: `${month} ${day}, ${year}`}
                notes.push(noteInfo);
                localStorage.setItem("notes", JSON.stringify(notes));

            });

        } else {
            // Fehlerbehandlung, wenn die Anfrage fehlschlägt
            console.error('Fehler beim Abrufen der Notizen. Statuscode: ' + response.status);
        }
    } catch (error) {
        console.error('Fehler beim Abrufen der Notizen:', error);
    }
}


async function deleteNotesFromServer(id){
    // URL der Ressource, die gelöscht werden soll
    /*const url = 'http://10.10.3.7:8080/api/notiz/' + id;

    // Senden der DELETE-Anfrage
    axios.delete(url)
        .then(response => {
            // Erfolgreiche Antwort erhalten
            console.log('Ressource erfolgreich gelöscht:', response);
        })
        .catch(error => {
            // Fehler beim Senden der Anfrage oder Verarbeiten der Antwort
            console.error('Fehler beim Löschen der Ressource:', error);
        });
     */

    try{
        const response = await fetch('http://10.10.3.7:8080/api/notiz/' + id, {
            method: 'DELETE'
        });

        // Überprüfen, ob die Anfrage erfolgreich war (Statuscode 200)
        if (response.ok) {
            console.log('Ressource erfolgreich gelöscht:', response)
        } else {
            // Fehlerbehandlung, wenn die Anfrage fehlschlägt
            console.error('Fehler beim Abrufen der Notizen. Statuscode: ' + response.status);
        }

    } catch (error) {
        console.error('Fehler beim Löschen der Notizen:', error);
    }


}

