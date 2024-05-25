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
                            <input type="checkbox" id="checkbox-${id}" class="note-checkbox" ${note.check ? 'checked' : ''}>
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

        let checkboxId = `checkbox-${id}`;
        let checkbox = document.getElementById(checkboxId);

        // Event-Listener hinzufügen, um auf Änderungen des Kontrollkästchens zu reagieren
        checkbox.addEventListener('change', function () {
            // Überprüfen, ob das Kontrollkästchen jetzt angekreuzt ist
            if (this.checked) {
                // Wenn das Kontrollkästchen angekreuzt ist, setze die Variable auf einen bestimmten Wert
                note.check = "true"; // Hier kannst du einen beliebigen Wert setzen
            } else {
                // Wenn das Kontrollkästchen nicht angekreuzt ist, setze die Variable auf einen anderen Wert
                note.check = "false"; // Hier kannst du einen anderen Wert setzen
            }

            // Jetzt kannst du die Variable verwenden oder mit dem neuen Wert arbeiten
            console.log('isChecked:', note.check);

            let json = `{
                "id": "${note.id}",
                "title": "${note.title}",
                "text": "${note.description.replace(/\n/g, ";")}",
                "check": "${note.check}"
            }`;
            putNotesOnServer(json);
        });
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
    /*let title = titleTag.value.trim(),
        description = descTag.value.trim();*/
    let title = getFormattedTimestamp(),
        description = descTag.value.trim().replace(/\n/g, ";");
    if(title || description) {
        let currentDate = new Date(),
            month = months[currentDate.getMonth()],
            day = currentDate.getDate(),
            year = currentDate.getFullYear();
        let noteInfo = {title, description, date: `${month} ${day}, ${year}`};
        if(!isUpdate) {
            notes.push(noteInfo);
            let json = `{
                "title": "${title}",
                "text": "${description}",
                "check": false
            }`;
            postNotesOnServer(json);
        } else {
            isUpdate = false;
            const noteIdMongo = notes[updateId].id;
            const noteCheckMongo = notes[updateId].check;
            const noteTitle = notes[updateId].title;
            notes[updateId] = noteInfo;
            let json = `{
                "id": "${noteIdMongo}",
                "title": "${noteTitle}",
                "text": "${description}",
                "check": "${noteCheckMongo}"
            }`;
            putNotesOnServer(json);
        }




        localStorage.setItem("notes", JSON.stringify(notes));
        showNotes();
        closeIcon.click();
        location.reload();
    }
});




async function fetchNotesFromServer() {
    try {
        // Senden einer GET-Anfrage an den Server
        //const response = await fetch('http://10.10.3.7:8080/api/notizen');
        const response = await fetch('/api/notizen');

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

    try{
        const response = await fetch('/api/notiz/' + id, {
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


async function postNotesOnServer(json){

    try{
        const response = await fetch('/api/notiz', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: json
        });

        // Überprüfen, ob die Anfrage erfolgreich war (Statuscode 200)
        if (response.ok) {
            console.log('Ressource erfolgreich erstellt:', response)
        } else {
            // Fehlerbehandlung, wenn die Anfrage fehlschlägt
            console.error('Fehler beim Abrufen der Notizen. Statuscode: ' + response.status);
        }

    } catch (error) {
        console.error('Fehler beim Löschen der Notizen:', error);
    }
}


async function putNotesOnServer(json){

    try{
        const response = await fetch('/api/notiz', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: json
        });

        // Überprüfen, ob die Anfrage erfolgreich war (Statuscode 200)
        if (response.ok) {
            console.log('Ressource erfolgreich geändert:', response)
        } else {
            // Fehlerbehandlung, wenn die Anfrage fehlschlägt
            console.error('Fehler beim Abrufen der Notizen. Statuscode: ' + response.status);
        }

    } catch (error) {
        console.error('Fehler beim Löschen der Notizen:', error);
    }
}

function getFormattedTimestamp() {
    const now = new Date();
    const day = String(now.getDate()).padStart(2, '0');
    const month = String(now.getMonth() + 1).padStart(2, '0'); // Monate sind nullbasiert
    const year = now.getFullYear();
    const hours = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');

    return `${day}.${month}.${year}, ${hours}:${minutes}:${seconds}`;
}




