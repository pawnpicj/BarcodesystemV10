
console.log("Page path is: " + window.location.pathname);
var fpath = window.location.pathname
var fjs;
var character = fpath.substring(1, 6);
var strURL = character.toLowerCase();
if (strURL === "sapui") {
    fjs = "http://203.151.171.140/" + character + "/js/setup-inc.json";
} else {
    fjs = "../../js/setup-inc.json";
}
