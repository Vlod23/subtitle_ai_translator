function showToast(message, isSuccess = true) {
    const toastEl = document.getElementById('toastMessage');
    const toastBody = document.getElementById('toastBody');

    if (!toastEl || !toastBody || !message?.trim()) return; // ✅ prevent blank toast

    toastBody.textContent = message;
    toastEl.classList.remove('bg-success', 'bg-danger', 'js-toast-triggered');
    toastEl.classList.add(isSuccess ? 'bg-success' : 'bg-danger', 'js-toast-triggered');

    toastEl.style.display = 'block';
    const toast = new bootstrap.Toast(toastEl);
    toast.show();
}


// Show Razor-generated message automatically
document.addEventListener("DOMContentLoaded", function () {
    const toastEl = document.getElementById("toastMessage");
    const toastBody = document.getElementById("toastBody");

    // This ensures it only shows when Razor set a message
    if (toastEl && toastBody && toastBody.innerText.trim() !== "" && !toastEl.classList.contains("js-toast-triggered")) {
        toastEl.style.display = "block";
        const toast = new bootstrap.Toast(toastEl);
        toast.show();
    }
});

// Auto-updating credits value after payment (maybe not needed)
//document.addEventListener("DOMContentLoaded", function () {
//    const creditDisplay = document.getElementById("userCredits");
//    if (!creditDisplay) return;

//    // Extract the number (e.g., from "💳 120 credits")
//    const creditText = creditDisplay.textContent.trim();
//    const matches = creditText.match(/(\d+)/);
//    const originalCredits = matches ? parseInt(matches[1]) : null;

//    if (!originalCredits) return;
    
//    const pollInterval = setInterval(() => {
//        fetch("/Credits/GetCreditBalance")
//            .then(res => res.json())
//            .then(data => {
//                if (data.credits > originalCredits) {
//                    creditDisplay.textContent = `💳 ${data.credits} credits`;
//                    clearInterval(pollInterval);
//                    showToast("Credits updated!", true);
//                }
//            })
//            .catch(err => console.error("Credit poll failed", err));
//    }, 5000);
//});
