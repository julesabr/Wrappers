// Licensed under the MIT license. See LICENSE file in the project root for full license information.

const darkColors = "colors-dark.css";
const lightColors = "colors-light.css";
const darkCode = "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/10.1.1/styles/night-owl.min.css";
const lightCode = "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.6.0/styles/base16/github.min.css";

let sw = document.getElementById('switch-dark');

function setColors(currentColors) {
    const path = colorsTheme.getAttribute('href').split('/');
    colorsTheme.setAttribute('href', path.slice(0, path.length - 1).concat(currentColors).join('/'));
}

function setCode(currentCode) {
    codeTheme.setAttribute('href', currentCode);
}

function toggleTheme(dark) {
    let currentColors = darkColors;
    let currentCode = darkCode;
    if (!dark) {
        currentColors = lightColors;
        currentCode = lightCode;
    }

    setColors(currentColors);
    setCode(currentCode);
}

function onChange() {
    toggleTheme(this.checked);
    if (this.checked)
        localStorage.setItem("theme", "dark");
    else 
        localStorage.setItem("theme", "light");
}

if (window.localStorage) {
    if(localStorage.getItem("theme") === "light") {
       sw.remove();
       sw = document.getElementById('switch-light');
    }
    else
       document.getElementById('switch-light').remove();

    sw.classList.remove("hidden");

    document.getElementById('theme').addEventListener("change", onChange);
}