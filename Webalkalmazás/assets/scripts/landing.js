let index = 1; // Az első valódi kép indexe
let autoSlideTimer;
let startX = 0;
let endX = 0;

const slider = document.querySelector(".slider");
const images = Array.from(document.querySelectorAll(".slider img"));
const totalSlides = images.length;

// Klónok létrehozása
const firstClone = images[0].cloneNode(true);
const lastClone = images[totalSlides - 1].cloneNode(true);

firstClone.setAttribute("id", "first-clone");
lastClone.setAttribute("id", "last-clone");

slider.appendChild(firstClone);
slider.insertBefore(lastClone, images[0]);

const allSlides = Array.from(document.querySelectorAll(".slider img"));
const totalSlidesWithClones = allSlides.length;

// Beállítjuk az alapértelmezett pozíciót az első valódi képre
slider.style.transform = `translateX(-${index * 100}%)`;

function moveSlide(direction) {
    index += direction;
    updateSliderPosition();

    // Ellenőrizzük, hogy klónra léptünk-e
    setTimeout(() => {
        if (allSlides[index].id === "first-clone") {
            jumpWithoutAnimation(1);
        } else if (allSlides[index].id === "last-clone") {
            jumpWithoutAnimation(totalSlides);
        }
    }, 500);

    resetAutoSlide();
}

function updateSliderPosition() {
    slider.style.transition = "transform 0.5s ease-in-out";
    slider.style.transform = `translateX(-${index * 100}%)`;
}

// Klónoknál azonnali visszaugrás az eredeti képre (ANIMÁCIÓ NÉLKÜL)
function jumpWithoutAnimation(targetIndex) {
    slider.style.transition = "none";
    index = targetIndex;
    slider.style.transform = `translateX(-${index * 100}%)`;

    setTimeout(() => {
        slider.style.transition = "transform 0.5s ease-in-out";
    }, 50);
}

// Automatikus lapozás újraindítása
function resetAutoSlide() {
    clearTimeout(autoSlideTimer);
    autoSlideTimer = setTimeout(() => moveSlide(1), 5000);
}

// Érintéses vezérlés (mobil)
slider.addEventListener("touchstart", (e) => {
    startX = e.touches[0].clientX;
});

slider.addEventListener("touchend", (e) => {
    endX = e.changedTouches[0].clientX;
    handleSwipe();
});

function handleSwipe() {
    let diff = startX - endX;
    if (diff > 50) {
        moveSlide(1);
    } else if (diff < -50) {
        moveSlide(-1);
    }
}

// Első automatikus lapozás beállítása
resetAutoSlide();