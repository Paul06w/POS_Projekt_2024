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

        checkbox.addEventListener('change', function () {
            if (this.checked) {
                note.check = "true";
            } else {
                note.check = "false";
            }
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
        //const response = await fetch('http://10.10.3.7:8080/api/notizen');
        const response = await fetch('/api/notizen');


        if (response.ok) {          //Anfrage erfolgreich (Statuscode 200)
            const notesData = await response.json();
            notesData.forEach(note => {
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

        if (response.ok) {
            console.log('Ressource erfolgreich gelöscht:', response)
        } else {
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

        if (response.ok) {
            console.log('Ressource erfolgreich erstellt:', response)
        } else {
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

        if (response.ok) {
            console.log('Ressource erfolgreich geändert:', response)
        } else {
            console.error('Fehler beim Abrufen der Notizen. Statuscode: ' + response.status);
        }

    } catch (error) {
        console.error('Fehler beim Ändern der Notizen:', error);
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




