/** @type {import('tailwindcss').Config} */
const defaultTheme = require("tailwindcss/defaultTheme");
module.exports = {
    content: ['./**/*.{razor,html}',
              "./Layout/**/*.razor"],
  theme: {
      extend: {
          colors: {
              pallette: {
                  "primary-black": "#5b595a",
                  "secondary-black": "#3e3838",
                  "hover-black": "#151313",
                  "disabled-black": "#484848"
              },
          },
      },
      screens: {
          xxs: "350px",
          xs: "475px",
          s: "570px",
          xmd: "888px",
          xlg: "980px",
          windowResize: "150px",
          wordWrap: "400px",
          inputResize: "492px",
          buttonResize: "368px",
          ...defaultTheme.screens,
      },
  },
  plugins: [],
}

