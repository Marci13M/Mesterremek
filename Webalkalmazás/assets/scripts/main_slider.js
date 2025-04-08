let index = 0;
let autoSlideTimer;

function moveSlide(direction) {
    const slider = document.querySelector(".slider");
    const images = document.querySelectorAll(".slider img");
    const totalSlides = images.length;

    index += direction;
    if (index < 0) index = totalSlides - 1;
    if (index >= totalSlides) index = 0;

    const translateValue = -index * 100 + "%";
    slider.style.transform = "translateX(" + translateValue + ")";

    resetAutoSlide();
}

function resetAutoSlide() {
    clearTimeout(autoSlideTimer);
    autoSlideTimer = setTimeout(() => moveSlide(1), 5000); 
}

resetAutoSlide();