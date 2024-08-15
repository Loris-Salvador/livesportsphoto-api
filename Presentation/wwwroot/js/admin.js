document.addEventListener("DOMContentLoaded", () => {
  const sectionsColumn = document.getElementById("sections-column");
  const albumsColumn = document.getElementById("albums-column");
  let selectedDiv;
  let selectedAbum;

  let button = document.getElementById("valider-button");
  let input = document.getElementById("textInput");

  let buttonAlbum = document.getElementById("valider-button-album");
  let inputAlbum = document.getElementById("textInputAlbum");
  let inputAlbumLink = document.getElementById("textInputAlbumLink");
  let deleteAlbumbutton = document.getElementById("delete-button-album");

  let deleteSectionButton = document.getElementById("section-button-section");

  let albumId;

  // Fonction pour charger les sections depuis l'API
  async function loadSections() {
    try {
      const response = await fetch("/api/Section"); // Remplacez par l'URL de votre API
      const sections = await response.json();

      // Vider le conteneur avant d'ajouter les nouvelles sections
      sectionsColumn.innerHTML = "";

      sections.forEach((section) => {
        const sectionDiv = document.createElement("div");
        sectionDiv.className = "section";
        sectionDiv.textContent = section.name;
        sectionDiv.dataset.id = section.id;

        sectionDiv.addEventListener("mouseover", () => {
          loadAlbums(section.id);

          if (selectedDiv && selectedDiv !== sectionDiv) {
            selectedDiv.style.backgroundColor = ""; // Réinitialiser la couleur de fond
          }

          // Appliquer la nouvelle couleur de fond
          sectionDiv.style.backgroundColor = "rgb(80, 80, 80)"; // Remplacez par la couleur souhaitée
          selectedDiv = sectionDiv;
        });

        sectionsColumn.appendChild(sectionDiv);
      });
    } catch (error) {
      console.error("Erreur lors du chargement des sections:", error);
    }
  }

  async function loadAlbums(sectionId) {
    try {
      const response = await fetch(
        `/api/section/album/${sectionId}`
      );
      const albums = await response.json();

      // Nettoyer la colonne des albums avant d'ajouter les nouveaux albums
      albumsColumn.innerHTML = "";

      albums.forEach((album) => {
        const albumDiv = document.createElement("div");
        albumDiv.className = "album";

        // Créer un élément <a> avec l'URL du lien
        const albumLink = document.createElement("p");
        albumLink.target = "_blank";
        albumLink.textContent = album.name;
        albumId = album.id;

        // Ajouter l'élément <a> au <div>
        albumDiv.appendChild(albumLink);

        // Ajouter un gestionnaire d'événements pour rediriger vers le lien lors du clic sur le <div>
        albumDiv.addEventListener("click", () => {
          if (selectedAbum && selectedAbum !== albumDiv) {
            selectedAbum.style.backgroundColor = ""; // Réinitialiser la couleur de fond
          }

          // Appliquer la nouvelle couleur de fond
          albumDiv.style.backgroundColor = "rgb(80, 80, 80)"; // Remplacez par la couleur souhaitée
          selectedAbum = albumDiv;
        });

        // Ajouter le <div> à la colonne des albums
        albumsColumn.appendChild(albumDiv);
      });
    } catch (error) {
      console.error("Erreur lors du chargement des albums:", error);
    }
  }

  // Charger les sections lors du chargement de la page
  loadSections();

  //input

  button.addEventListener("click", async () => {
    console.log("click");
    const name = input.value;
    console.log(name);
    const url = `/api/section?name=${name}`;

    https: await fetch(url, {
      method: "POST", // Méthode HTTP
      headers: {
        "Content-Type": "text/plain", // Spécifier le type de contenu
      },
    });
    console.log("load sections");
    await loadSections();
  });

  buttonAlbum.addEventListener("click", async () => {
    console.log("click");
    const name = inputAlbum.value;
    console.log(selectedDiv.dataset.id);
    const url = `/api/section/album?sectionId=${selectedDiv.dataset.id}`;

    const data = {
      name: name,
      link: inputAlbumLink.value,
    };

    https: await fetch(url, {
      method: "POST", // Méthode HTTP
      headers: {
        "Content-Type": "application/json", // Spécifier le type de contenu
      },
      body: JSON.stringify(data), // Convertir l'objet de données en chaîne JSON
    });
    await loadAlbums(selectedDiv.dataset.id);
  });

  deleteAlbumbutton.addEventListener("click", async () => {
    const url = `/api/section/album?sectionId=${selectedDiv.dataset.id}&albumId=${albumId}`;

    https: await fetch(url, {
      method: "DELETE", // Méthode HTTP
      headers: {},
    });
    await loadAlbums(selectedDiv.dataset.id);
  });

  deleteSectionButton.addEventListener("click", async () => {
    console.log("click");
    const url = `/api/section?sectionId=${selectedDiv.dataset.id}`;

    https: await fetch(url, {
      method: "DELETE", // Méthode HTTP
      headers: {},
    });
    await loadSections();
  });
});
