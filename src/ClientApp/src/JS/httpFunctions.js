async function sendAsync(endpoint, request){
    const response = await fetch(endpoint, request);
    if (!response.ok) {
        const text = await response.text()
        throw new Error(text);
    }
    return response.json();
}