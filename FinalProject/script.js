document.addEventListener('DOMContentLoaded', () => {

    /* =========================================
       PART 1: LOGIN PAGE LOGIC
       ========================================= */
    const loginForm = document.getElementById('loginForm');

    // Check if we are on the login page
    if (loginForm) {
        loginForm.addEventListener('submit', (event) => {
            
            // 1. Stop the page from reloading
            event.preventDefault(); 
            
            // 2. Get the input boxes
            const userBox = document.getElementById('loginUser');
            const passBox = document.getElementById('loginPass');
            
            // 3. Get the text inside them
            const username = userBox.value;
            const password = passBox.value;

            // 4. Basic Validation
            if (username === "" || password === "") {
                alert("Please fill in both fields!");
                return; 
            }

            // 5. REDIRECT: Add the username to the URL
            // This creates a URL like: dashboard.html?username=Rahul
            window.location.href = "dashboard.html?username=" + username;
        });
    }

    /* =========================================
       PART 2: DASHBOARD PAGE LOGIC
       ========================================= */
    // Check if we are on the dashboard page
    const nameDisplay = document.getElementById('userNameDisplay');

    if (nameDisplay) {
        
        // 1. Read the current URL parameters (the part after ?)
        let params = new URLSearchParams(window.location.search);
        
        // 2. Extract the 'username' value
        let name = params.get("username");

        // 3. Display the name on the screen
        if (name) {
            nameDisplay.innerText = name;
        } else {
            nameDisplay.innerText = "Guest";
        }
    }

    /* =========================================
       PART 3: REGISTRATION PAGE LOGIC (Simple)
       ========================================= */
    const regForm = document.getElementById('regForm');

    if (regForm) {
        regForm.addEventListener('submit', (event) => {
            event.preventDefault(); // Stop reload
            
            // Show the hidden output section
            document.getElementById('outputSection').style.display = 'block';
            
            // Get values
            const name = document.getElementById('regName').value;
            const email = document.getElementById('regEmail').value;
            const course = document.getElementById('regCourse').value;
            
            // Show values below form
            const outputDiv = document.getElementById('outputData');
            outputDiv.innerHTML = `
                <p><strong>Name:</strong> ${name}</p>
                <p><strong>Email:</strong> ${email}</p>
                <p><strong>Course:</strong> ${course}</p>
            `;
        });
    }
});