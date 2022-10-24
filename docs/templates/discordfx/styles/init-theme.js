// Licensed under the MIT license. See LICENSE file in the project root for full license information.

let colorsTheme = document.getElementById('colors-dark');
let codeTheme = document.getElementById('code-dark');

if (window.localStorage) {
    if(localStorage.getItem("theme") === "light") {
        colorsTheme.remove();
        codeTheme.remove();
        colorsTheme = document.getElementById('colors-light');
        codeTheme = document.getElementById('code-light');
    }
    else {
        document.getElementById('colors-light').remove();
        document.getElementById('code-light').remove();
    }

    colorsTheme.classList.remove("hidden");
    codeTheme.classList.remove("hidden");
}