
const searchWrapper = document.querySelector(".search-input");
const inputBox = searchWrapper.querySelector("input");
const suggBox = searchWrapper.querySelector(".autocom-box");

inputBox.onkeyup = (e) => {
    let userData = e.target.value;
    let emptyArray = [];
    if (userData) {
        $.ajax({
            type: 'GET',
            crossDomain: true,
            dataType: 'jsonp',
            url: "http://suggestqueries.google.com/complete/search?client=youtube&ds=yt&q=" + userData,
            success: function (data) {
                data = data[1];
                data.forEach(function (item) {
                    emptyArray.push(item[0]);
                });
                emptyArray = emptyArray.map((data) => {
                    return data = '<li>' + data + '</li>';
                });
                searchWrapper.classList.add("active");
                showSuggestions(emptyArray);
                let allList = suggBox.querySelectorAll("li");
                for (let i = 0; i < allList.length; i++) {
                    allList[i].setAttribute("onclick", "select(this)");
                }
            }
        });
    } else {
        searchWrapper.classList.remove("active");
    }
}

function select(element) {
    let selectData = element.textContent;
    inputBox.value = selectData;
    searchWrapper.classList.remove("active");
    $("#search").submit();
}

function showSuggestions(list) {
    let listData;
    if (!list.length) {
        userValue = inputBox.value;
        listData = '<li>' + userValue + '</li>';
    } else {
        listData = list.join('');
    }
    suggBox.innerHTML = listData;
}