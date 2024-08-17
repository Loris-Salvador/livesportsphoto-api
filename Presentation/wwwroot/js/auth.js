document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("loginForm");

  form.addEventListener("submit", async (event) => {
    event.preventDefault(); // Empêche le rechargement de la page

    // Récupère les valeurs des champs de formulaire
    const username = encodeURIComponent(
      document.getElementById("username").value
    );
    const password = encodeURIComponent(
      document.getElementById("password").value
    );

    try {
      // Requête pour obtenir le JWT
      const response = await fetch("/api/auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ username, password }),
      });

      if (!response.ok) {
        throw new Error("Erreur d'authentification");
      }

      const data = await response.json();
      const token = data.token;

      // Stocker le JWT dans localStorage
      localStorage.setItem("jwtToken", token);

      // Faire une requête GET vers /admin/index avec le JWT
      const indexResponse = await fetch("/admin/content", {
        method: "GET",
        headers: {
          "Authorization": `Bearer ${token}`,
        },
      });

      if (indexResponse.ok) {
        // Récupérer le contenu HTML de la réponse
        const htmlContent = await indexResponse.text();
        
        // Remplacer le contenu de la page par le HTML reçu
        document.open();
        document.write(htmlContent);
        document.close();
      } else {
        throw new Error("Accès non autorisé");
      }
    } catch (error) {
      console.error("Erreur :", error.message);
    }
  });
});
