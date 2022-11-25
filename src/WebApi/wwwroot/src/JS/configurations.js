/**
 * @returns promise to forum configurations
 */
const getConfiguration = ()=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(URLS.Configs, request);
};
/**Changes configuration parameters as rules, warningCountForBan, banDuration, complaintDuration
 * @returns answer from server
 */
const updateConfiguration = (configuration)=>{
    const request = {
        method: "PATCH",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(configuration)
    };
    return sendAsync(URLS.Configs, request);
};
/**
 * @returns promise to phrases array
 * @param {String} type - type of words
 */
const getPhrases = (type)=>{
    const request = {
        method: "GET",
        headers: {
            "Content-Type": "application/json"
        }
    };
    return sendAsync(URLS.Phrases + `/${type}`, request);
};
/**
 * @returns promise to forbidden phrases array
 */
const getForbiddenPhrases = ()=>{
    return getPhrases("Forbidden");
};
/**
 * @returns promise to verification required phrases array
 */
const getVerifyPhrases = ()=>{
    return getPhrases("VerificationRequired");
};
/**
 * Updates words
 * @param {Array} words - array with words
 * @param {String} type - type of words
 * @returns answer from server
 */
const updatePhrases = (words, type)=>{
    const body = words.map(item=>{return {"phrase":item}});
    const request = {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization":"Bearer "+getFromStorage("token")
        },
        body: JSON.stringify(body)
    };
    return sendAsync(URLS.Phrases + `/${type}/Bulk`, request);
};
/**
 * @returns answer from server
 */
const updateForbiddenPhrases = (words)=>{
    return updatePhrases(words, "Forbidden");
};
/**
 * @returns answer from server
 */
const updateVerifyPhrases = (words)=>{
    return updatePhrases(words, "VerificationRequired");
};