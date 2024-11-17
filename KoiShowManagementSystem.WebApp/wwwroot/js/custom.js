// Wait for the DOM to be fully loaded
document.addEventListener("DOMContentLoaded", function () {

    // Navbar color change on scroll
    const navbar = document.querySelector(".navbar");
    window.addEventListener("scroll", function () {
        if (window.scrollY > 50) {
            navbar.classList.add("scrolled");
        } else {
            navbar.classList.remove("scrolled");
        }
    });

    // Animate the buttons when hovering
    const buttons = document.querySelectorAll(".btn-custom");
    buttons.forEach(button => {
        button.addEventListener("mouseover", () => {
            button.classList.add("hovered");
        });
        button.addEventListener("mouseout", () => {
            button.classList.remove("hovered");
        });
    });
});
