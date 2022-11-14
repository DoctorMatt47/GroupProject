const BASE_URL = "http://localhost:5000/api/";
const URLS = {
    UsersAuthenticate:BASE_URL + "Identities/Authenticate",
    Users:BASE_URL + "Users",
    Topics:BASE_URL + "Topics",
    TopicsCreate:BASE_URL + "Topics/InSection/",
    Section: BASE_URL + "Section",
    Comments: BASE_URL + "Commentaries",
    CommentsCreate: BASE_URL + "Commentaries/OnTopic/",
    ComplaintTopicCreate: BASE_URL + "Complaints/OnTopic/",
    ComplaintCommentCreate: BASE_URL + "Complaints/OnCommentary/",
    ComplaintByTopic: BASE_URL + "Complaints/ByTopic/",
    ComplaintByComment: BASE_URL + "Complaints/ByCommentary/",
};