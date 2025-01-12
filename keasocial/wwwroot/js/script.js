const baseUrl = 'http://localhost:5260/';
initButtons();

function initButtons() {
    const getPostForm = document.querySelector("#get-post-form");
    const getAllButton = document.querySelector("#get-all-posts");
    const clearButton = document.querySelector("#clear-posts");
    const getWeatherButton = document.querySelector("#get-weather");

    getPostForm.addEventListener("submit", async (e) => {
        e.preventDefault();
        const id = getPostForm.number.valueAsNumber;

        if (isNaN(id)) {
            return;
        }

        await fetchOnePost(id)
    });

    getAllButton.addEventListener("click", async () => {
        await fetchAllPosts();
    });
    
    clearButton.addEventListener("click", () => {
        clearOutput()
    })

    getWeatherButton.addEventListener("click", async () => {
        await fetchWeather();
    })
}


async function fetchOnePost(id) {
    const endpoint = `api/Post/${id}`

    // API call
    fetch(baseUrl + endpoint)
        .then(response => {
            if (!response.ok) {
                handleError();
            } else {
                return response.json();
            }
        })
        .then(displayPost)
        .catch(handleError);
}

async function fetchAllPosts() {
    const endpoint = "api/Post"
    // API call
    fetch(baseUrl + endpoint)
        .then(response => {
            if (!response.ok) {
                handleError();
            } else {
                return response.json();
            }
        })
        .then(displayPosts)
        .catch(handleError);
}

function displayPosts(data) {
    clearOutput()

    data.forEach(post => {
        const postContainer = document.createElement('div');
        postContainer.classList.add('post');

        postContainer.innerHTML = `
            <h3>Post #${post.postId}</h3>
            <p><strong>User ID:</strong> ${post.userId}</p>
            <p><strong>Content:</strong> ${post.content}</p>
            <p><strong>Created At:</strong> ${new Date(post.createdAt).toLocaleString()}</p>
            <p><strong>Likes:</strong> ${post.likeCount}</p>
        `;

        output.appendChild(postContainer);
    });
}

function displayPost(post) {
    clearOutput()
    
    const postContainer = document.createElement('div');
    postContainer.classList.add('post');

    postContainer.innerHTML = `
        <h3>Post #${post.postId}</h3>
        <p><strong>User ID:</strong> ${post.userId}</p>
        <p><strong>Content:</strong> ${post.content}</p>
        <p><strong>Created At:</strong> ${new Date(post.createdAt).toLocaleString()}</p>
        <p><strong>Likes:</strong> ${post.likeCount}</p>
    `;

    output.appendChild(postContainer);
}

function clearOutput() {
    const output = document.querySelector("#output")
    output.innerHTML = ""
}

function handleError() {
    const output = document.querySelector('#output');

    output.innerHTML =
        '<p>There was a problem communicating with the API</p>';

    setTimeout(() => {
        output.innerHTML = '';
    }, 2000);
}


async function fetchWeather() {
    const endpoint = "api/Weather";
    fetch(baseUrl + endpoint)
        .then(response => {
            if (!response.ok) {
                handleError();
            } else {
                return response.json();
            }
        })
        .then(displayWeather)
        .catch(handleError);
}

function displayWeather(data) {
    const weatherElem = document.querySelector("#weather");
    weatherElem.innerHTML = `
conditions: ${data.description} | 
temperature: ${data.temp}°C | 
humidity: ${data.humidity}% | wind speeds: 
${data.speed}m/s</p>
    `;
}
