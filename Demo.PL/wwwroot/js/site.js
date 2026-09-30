// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

(function () {

    var searchInp = document.getElementById("searchInp");

    if (searchInp) {
        searchInp.addEventListener("search", function () {
            searchInp.form.submit();
        });
    }
})();
