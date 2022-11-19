const BASE_URL = "http://localhost:5000/api/";
const URLS = {
    UsersAuthenticate:BASE_URL + "Identities/Authenticate",
    Users:BASE_URL + "Users",
    Topics:BASE_URL + "Topics",
    TopicsCreate:BASE_URL + "Topics/InSection/",
    Section: BASE_URL + "Section",
    Comments: BASE_URL + "Commentaries",
    CommentsCreate: BASE_URL + "Commentaries/OnTopic/",
    ComplaintTopic: BASE_URL + "Complaints/AboutTopic/",
    ComplaintComment: BASE_URL + "Complaints/AboutCommentary/",
    Complaints: BASE_URL + "Complaints",
    Configs: BASE_URL + "Configuration",
    Phrases: BASE_URL + "Phrases",
};