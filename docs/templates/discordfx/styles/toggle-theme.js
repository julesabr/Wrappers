// Licensed under the MIT license. See LICENSE file in the project root for full license information.
const darkTheme = "colors-dark.css";
const lightTheme = "colors-light.css";
const darkCodeTheme = "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/10.1.1/styles/night-owl.min.css";
const lightCodeTheme = "https://cdnjs.cloudflare.com/ajax/libs/highlight.js/11.6.0/styles/base16/github.min.css";
const sw = document.getElementById('switch-style');

const theme = document.getElementById('theme');
const href = theme.getAttribute('href');
const path = href.split('/');

const codeTheme = document.getElementById('code-theme');

function setTheme(currentTheme) {
    theme.setAttribute('href', path.slice(0, path.length - 1).concat(currentTheme).join('/'));
}

function setCodeTheme(currentCodeTheme) {
    codeTheme.setAttribute('href', currentCodeTheme);
}

function toggleTheme() {
    let currentTheme = darkTheme;
    let currentCodeTheme = darkCodeTheme;
    if (!this.checked) {
        currentTheme = lightTheme;
        currentCodeTheme = lightCodeTheme;
    }

    setTheme(currentTheme);
    setCodeTheme(currentCodeTheme);

    if (window.localStorage) {
        localStorage.setItem("theme", currentTheme);
        localStorage.setItem("code-theme", currentCodeTheme);
    }
}

if (sw)
    sw.addEventListener("change", toggleTheme);

if (window.localStorage && window.localStorage.getItem("theme") && !href.includes(localStorage.getItem("theme"))) {
    setTheme(localStorage.getItem("theme"));
    setCodeTheme(localStorage.getItem("code-theme"));

    if (localStorage.getItem("theme") === lightTheme)
        sw.checked = false;
}