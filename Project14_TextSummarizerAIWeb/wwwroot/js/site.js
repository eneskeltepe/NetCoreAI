// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// ===== GLASSMORPHISM AI TEXT SUMMARIZER JS =====

// Initialize when DOM is loaded
document.addEventListener('DOMContentLoaded', function() {
    initializeAnimations();
    initializeParticles();
    initializeFormEffects();
});

// Floating Particles Animation
function initializeParticles() {
    // Create floating particles dynamically
    const particlesContainer = document.querySelector('.particles');
    if (!particlesContainer) {
        // Create particles container if it doesn't exist
        const container = document.createElement('div');
        container.className = 'particles';
        document.body.appendChild(container);
    }

    // Add CSS for particles
    const particleStyles = `
        .particles {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            overflow: hidden;
            z-index: -1;
            pointer-events: none;
        }

        .particle {
            position: absolute;
            background: rgba(0, 212, 255, 0.1);
            border-radius: 50%;
            animation: float 15s infinite linear;
        }

        .particle:nth-child(1) {
            width: 20px;
            height: 20px;
            top: 20%;
            left: 20%;
            animation-delay: 0s;
            animation-duration: 20s;
        }

        .particle:nth-child(2) {
            width: 15px;
            height: 15px;
            top: 60%;
            left: 80%;
            animation-delay: 2s;
            animation-duration: 18s;
        }

        .particle:nth-child(3) {
            width: 25px;
            height: 25px;
            top: 80%;
            left: 10%;
            animation-delay: 4s;
            animation-duration: 22s;
        }

        .particle:nth-child(4) {
            width: 12px;
            height: 12px;
            top: 30%;
            left: 70%;
            animation-delay: 6s;
            animation-duration: 16s;
        }

        .particle:nth-child(5) {
            width: 18px;
            height: 18px;
            top: 70%;
            left: 30%;
            animation-delay: 8s;
            animation-duration: 24s;
        }

        @keyframes float {
            0% {
                transform: translateY(0px) rotate(0deg);
                opacity: 0;
            }
            10% {
                opacity: 0.8;
            }
            90% {
                opacity: 0.8;
            }
            100% {
                transform: translateY(-100vh) rotate(360deg);
                opacity: 0;
            }
        }
    `;

    // Add styles to head
    const style = document.createElement('style');
    style.textContent = particleStyles;
    document.head.appendChild(style);
}

// Initialize Animations
function initializeAnimations() {
    // Intersection Observer for scroll animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
                entry.target.style.transition = 'all 0.6s ease-out';
            }
        });
    }, observerOptions);

    // Observe all elements with fade-in-up class
    document.querySelectorAll('.fade-in-up').forEach(el => {
        el.style.opacity = '0';
        el.style.transform = 'translateY(30px)';
        observer.observe(el);
    });

    // Stagger animation for cards
    const cards = document.querySelectorAll('.summary-card');
    cards.forEach((card, index) => {
        card.style.animationDelay = `${index * 0.1}s`;
        card.classList.add('fade-in-up');
    });
}

// Form Effects
function initializeFormEffects() {
    // Add focus effects to form controls
    const formControls = document.querySelectorAll('.form-control, .form-select');
    formControls.forEach(control => {
        control.addEventListener('focus', function() {
            this.parentElement.classList.add('focused');
            addRippleEffect(this);
        });

        control.addEventListener('blur', function() {
            this.parentElement.classList.remove('focused');
        });
    });


}

// Ripple Effect
function addRippleEffect(element) {
    const ripple = document.createElement('div');
    ripple.className = 'ripple';
    
    const rippleStyles = `
        position: absolute;
        border-radius: 50%;
        background: rgba(0, 212, 255, 0.3);
        transform: scale(0);
        animation: ripple 0.6s ease-out;
        pointer-events: none;
    `;
    
    ripple.style.cssText = rippleStyles;
    
    const rect = element.getBoundingClientRect();
    const size = Math.max(rect.width, rect.height);
    ripple.style.width = ripple.style.height = size + 'px';
    ripple.style.left = '50%';
    ripple.style.top = '50%';
    ripple.style.transform = 'translate(-50%, -50%) scale(0)';
    
    element.style.position = 'relative';
    element.appendChild(ripple);
    
    // Add ripple animation
    const rippleAnimation = `
        @keyframes ripple {
            to {
                transform: translate(-50%, -50%) scale(2);
                opacity: 0;
            }
        }
    `;
    
    setTimeout(() => {
        ripple.remove();
    }, 600);
}

// Button Loading State
function setButtonLoading(button, isLoading) {
    const text = button.querySelector('.btn-text') || button;
    const loader = button.querySelector('.loading-dots');
    
    if (isLoading) {
        button.disabled = true;
        text.style.opacity = '0';
        if (loader) {
            loader.style.display = 'inline-block';
        }
        button.style.cursor = 'not-allowed';
    } else {
        button.disabled = false;
        text.style.opacity = '1';
        if (loader) {
            loader.style.display = 'none';
        }
        button.style.cursor = 'pointer';
    }
}

// Copy to Clipboard with Animation
function copyToClipboard(text, button) {
    navigator.clipboard.writeText(text).then(() => {
        // Show success feedback
        const originalText = button.textContent;
        button.textContent = '✓ Kopyalandı!';
        button.style.color = '#00ff88';
        
        setTimeout(() => {
            button.textContent = originalText;
            button.style.color = '';
        }, 2000);
    }).catch(err => {
        console.error('Kopyalama başarısız:', err);
    });
}

// Smooth Scroll to Results
function scrollToResults() {
    const resultsSection = document.querySelector('.summary-card');
    if (resultsSection) {
        resultsSection.scrollIntoView({ 
            behavior: 'smooth', 
            block: 'start' 
        });
    }
}

// Theme Toggle (if needed later)
function toggleTheme() {
    document.body.classList.toggle('light-theme');
    localStorage.setItem('theme', 
        document.body.classList.contains('light-theme') ? 'light' : 'dark'
    );
}

// Initialize theme from localStorage
function initializeTheme() {
    const savedTheme = localStorage.getItem('theme');
    if (savedTheme === 'light') {
        document.body.classList.add('light-theme');
    }
}

// Add typing effect to text
function typeWriter(element, text, speed = 50) {
    let i = 0;
    element.textContent = '';
    
    function type() {
        if (i < text.length) {
            element.textContent += text.charAt(i);
            i++;
            setTimeout(type, speed);
        }
    }
    
    type();
}

// Parallax effect for background
function initializeParallax() {
    window.addEventListener('scroll', () => {
        const scrolled = window.pageYOffset;
        const rate = scrolled * -0.5;
        
        const particles = document.querySelector('.particles');
        if (particles) {
            particles.style.transform = `translateY(${rate}px)`;
        }
    });
}

// Export functions for global use
window.GlassUI = {
    setButtonLoading,
    copyToClipboard,
    scrollToResults,
    toggleTheme,
    typeWriter
};
