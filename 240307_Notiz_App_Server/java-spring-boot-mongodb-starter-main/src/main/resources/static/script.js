const notesContainer = document.querySelector(".notes-container");
const createBtn = document. querySelector(".btn");
let notes = document.querySelectorAll(".input-box");

function showNotes(){
    notesContainer.innerHTML = localStorage.getItem("notes");
}
showNotes();

function updateStorage(){
    localStorage.setItem("notes", notesContainer.innerHTML)
}

createBtn.addEventListener("click", ()=>{
    let inputBox = document. createElement("p");
    let img = document.createElement("img");
    let checkbox = document.createElement("input");

    inputBox.className = "input-box";
    inputBox. setAttribute("contenteditable", "true");

    img.src = "/delete.png";

    checkbox.type = "checkbox";

    notesContainer.appendChild(inputBox).appendChild(img);

    //img.insertAdjacentElement("afterend", checkbox);
})

notesContainer.addEventListener("click", function(e) {
    if (e.target.tagName === "IMG") {
        e.target.parentElement.remove();
        updateStorage()
    }
    else if(e.target.tagName === "P") {
        notes = document.querySelectorAl1(".input-box");
        notes.forEach(nt => {
            nt.onkeyup = function () {
                updateStorage();
            }
        })
    }
})

document. addEventListener("keydown", event => {
    if (event.key === "Enter") {
        document.execCommand("insertLineBreak");
        event.preventDefault();
    }
})

function addSection(id, title, text, check){
    let inputBox = document. createElement("p");
    let img = document.createElement("img");
    let checkbox = document.createElement("input");

    inputBox.className = "input-box";
    inputBox. setAttribute("contenteditable", "true");

    img.src = "/delete.png";

    checkbox.type = "checkbox";


    inputBox.textContent = text.replace(/;/g, "<br><br>");

    notesContainer.appendChild(inputBox).appendChild(img);
}



async function fetchNotesFromServer() {
    try {
        // Senden einer GET-Anfrage an den Server
        const response = await fetch('http://10.10.3.7:8080/api/notizen');

        // Überprüfen, ob die Anfrage erfolgreich war (Statuscode 200)
        if (response.ok) {
            // Konvertieren der Antwort in JSON
            const notesData = await response.json();

            // Anzeigen der Notizen im Notiz-Container
            notesContainer.innerHTML = '';
            notesData.forEach(note => {
                //const noteElement = document.createElement("p");
                addSection(note.id, note.title, note.text, note.check)
                //noteElement.textContent = note.title;
                //notesContainer.appendChild(noteElement);
            });
        } else {
            // Fehlerbehandlung, wenn die Anfrage fehlschlägt
            console.error('Fehler beim Abrufen der Notizen. Statuscode: ' + response.status);
        }
    } catch (error) {
        console.error('Fehler beim Abrufen der Notizen:', error);
    }
}

// Aufrufen der Funktion zum Abrufen der Notizen, wenn die Seite geladen wird
fetchNotesFromServer();