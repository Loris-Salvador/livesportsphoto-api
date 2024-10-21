const albums = document.querySelectorAll('.album');

albums.forEach(album => {
    album.addEventListener('click', function() {
        const link = album.getAttribute('data-link');
        if (link) {
            window.location.href = link;
        }
    });
});

