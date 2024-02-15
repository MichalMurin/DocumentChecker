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
          shopItemResize: "410px",
          xxs: "350px",
          xs: "475px",
          s: "570px",
          ggg: "540px",
          xxmd: "500px",
          xmd: "888px",
          xlg: "980px",
          headerResize: "650px",
          larrge: "1200px",
          larrrge: "1380px",
          gallery: "1300px",
          cards: "950px",
          text: "790px",
          contactResize: "750px",
          footerResize: "1060px",
          numbers: "1250px",
          imageResize: "1550px",
          textResize: "1150px",
          ownerText: "800px",
          pictures: "600px",
          cardOpen: "728px",
          productResize: "697px",
          menuResize: "1000px",
          hamburgerSize: "840px",
          submitOrder: "904px",
          shopItemLine: "1400px",
          dotsRemove: "1420px",
          paralaxResize: "1450px",
          ...defaultTheme.screens,
      },
  },
  plugins: [],
}

