document.addEventListener("DOMContentLoaded", () => {
  const form = document.getElementById("loginForm");

  form.addEventListener("submit", (event) => {
    event.preventDefault(); // Empêche le rechargement de la page

    // Récupère les valeurs des champs de formulaire
    const username = encodeURIComponent(
      document.getElementById("username").value
    );
    const password = encodeURIComponent(
      document.getElementById("password").value
    );

    // Crée l'URL avec les paramètres encodés
    const url = `/admin/index?name=${username}&password=${password}`;

    // Effectue la redirection
    window.location.href = url;
  });
});
