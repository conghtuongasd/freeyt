var ClipboardHelper = {

    copyElement: function ($element) {
        this.copyText($element.text());
    },
    copyText: function (text) // Linebreaks with \n
    {
        var $tempInput = $("<textarea>");
        $("body").append($tempInput);
        $tempInput.val(text).select();
        document.execCommand("copy");
        $tempInput.remove();
    }
};
var homeIndex = function () {
    var dom = {
        btnGetVideo: $("#get-video"),
        btnSearchButton: $("#search-video"),
        txtKey: $("#key"),
        listVideos: $("#list-videos")
    }

    var videoLists = null; 

    var getUrlParameter = function getUrlParameter(sParam) {
        var sPageURL = window.location.search.substring(1),
            sURLVariables = sPageURL.split('&'),
            sParameterName,
            i;

        for (i = 0; i < sURLVariables.length; i++) {
            sParameterName = sURLVariables[i].split('=');

            if (sParameterName[0] === sParam) {
                return sParameterName[1] === undefined ? true : decodeURIComponent(sParameterName[1]);
            }
        }
    };

    function _htmlEncode(value) {
        return $('<div>').text(value.replace(/"/g, '&quot')).html();
    }

    function _init() {
        if (!localStorage.getItem("ListVideo")) {
            storage.setItem("ListVideo", []);
            videoLists = [];
        }
        if (!videoLists && localStorage.getItem("ListVideo")) {
            videoLists = storage.getItem("ListVideo");
        }

        if ($("#url").val()) {
            setTimeout(function () { dom.btnGetVideo.click(); }, 100);
        }
    }

    function _bindEvents() {
        dom.btnGetVideo.on("click", function () {
            $("#video-control").html('');
            $("#list-videos").html('');
            $("#video-title").text('');
            dom.btnGetVideo.attr("disabled", true);
            $.get("/Home/GetLinks?url=" + $("#url").val(), function (data, status) {
                if (data.isSuccess) {
                    $(data.youtubeVideos).each(function (i, e) {
                        $("#list-videos").append('<span class="btn btn-warning" data-url="' + e.url + '"><i class="fa fa-play" aria-hidden="true"></i> ' + e.resolution + '</span>')
                    });
                    if (data.youtubeVideos.length > 0) {
                        $("#list-videos").append('<span class="btn btn-success share" data-url="' + $("#url").val() + '"><i class="fa fa-send"></i> Share</span>')
                    }
                    $("#video-control").html('<video autoplay="autoplay" src="' + data.youtubeVideos[0].url + '" class="col-12" controls="controls"></video>')
                    $("#video-title").text(data.title);
                    $("#list-videos").get(0).scrollIntoView();
                    _createEndedEvent();
                } else {
                    alert("Can't get this video!");
                }
                dom.btnGetVideo.removeAttr("disabled");
            });
        });
        dom.listVideos.on("click", ".play-video", function () {
            $("#video-control").html('<video autoplay="autoplay" src="' + $(this).data("url") + '" class="col-12" controls="controls"></video>')
        });

        dom.listVideos.on("click", ".share", function (e) {
            e.preventDefault();
            ClipboardHelper.copyText(location.origin + "/?url="+$(e.target).data("url"));
            Core.alert("Sao chép liên kết thành công!", true);
        });

        dom.btnSearchButton.on("click", function () {
            $("#list-search").html('');
            $('#list-search').attr('data-key', $("#key").val());
            dom.btnSearchButton.attr("disabled", true);
            $.get("/Home/SearchVideos?key=" + $("#key").val(), function (data, status) {
                dom.btnSearchButton.removeAttr("disabled");
                if (data.videos.length == 0) {
                    $("#list-search").html('Không tìm thấy');
                } else {
                    $('#list-search').attr('data-continuation', data.continuation);
                    $(data.videos).each(function (i, e) {
                        var st = '<div class="video-item" data-id="' + e.videoRenderer.videoId + '" style="display: flex">'
                            + '    <div class="float-left image">'
                            + '        <img src="' + e.videoRenderer.thumbnail.thumbnails[0].url + '" width="100" />'
                            + '    </div>'
                            + '    <div class="float-left title">'
                            + '        <span>'
                            + e.videoRenderer.title.runs[0].text
                            + '        </span>'
                            + '        <small>Kênh: ' + e.videoRenderer.ownerText.runs[0].text + '</small>'
                            + '        <small>Thời gian: ' + (e.videoRenderer.lengthText ? e.videoRenderer.lengthText.simpleText : '') + ' - ' + (e.videoRenderer.viewCountText ? e.videoRenderer.viewCountText.simpleText : '') + '</small>'
                            + '    </div>'
                            + '    <div class="float-right action">'
                            + '         <span class="add-to-playlist" title="Thêm vào hàng đợi" data-id="' + e.videoRenderer.videoId + '" data-url="' + e.videoRenderer.thumbnail.thumbnails[0].url + '" data-title="' + _htmlEncode(e.videoRenderer.title.runs[0].text) + '"><i class="fa fa-indent" aria-hidden="true"></i></span>'
                            + '    </div'
                            + '</div>';
                        $('#list-search').append(st);
                    });
                }
            });
        });

        dom.txtKey.keyup(function () {
            if (dom.txtKey.val().length > 0) {
                dom.btnSearchButton.prop('disabled', false);
            } else {
                dom.btnSearchButton.prop('disabled', true);
            }
        });
        dom.txtKey.keypress(function (e) {
            if (e.charCode == 13) {
                dom.btnSearchButton.click();
            }
        })

        $("#list-search,#playlist-content").on("click", ".video-item", function (e) {
            e.preventDefault();
            e.stopImmediatePropagation();
            e.stopPropagation();
            $("#video-control").html('');
            $("#video-title").text('');
            $("#list-videos").html('');
            dom.btnGetVideo.attr("disabled", true);
            var dataId = $(this).data('id');
            _playVideoById(dataId);
            if ($(e.target).closest('#playlist-content').length != 0) {
                videoLists = videoLists.slice(1);
                storage.setItem('ListVideo', videoLists);
                _loadPlaylistVideo();
            }
        });

        $('.top-trending').on('click', function (e) {
            $("#list-search").html('');
            dom.btnSearchButton.attr("disabled", true);
            $.get("/Home/GetTopTrendingVideos?param=" + $(e.target).data('param'), function (data, status) {
                if (data.videos.length == 0) {
                    $("#list-search").html('Không tìm thấy');
                } else {
                    $('#list-search').attr('data-continuation', data.continuation);
                    $(data.videos).each(function (i, e) {
                        var st = '<div class="video-item" data-id="' + e.videoRenderer.videoId + '">'
                            + '    <div class="float-left image">'
                            + '        <img src="' + e.videoRenderer.thumbnail.thumbnails[0].url + '" width="100" />'
                            + '    </div>'
                            + '    <div class="float-left title">'
                            + '        <span>'
                            + e.videoRenderer.title.runs[0].text
                            + '        </span>'
                            + '        <small>Kênh: ' + e.videoRenderer.ownerText.runs[0].text + '</small>'
                            + '        <small>Thời gian: ' + (e.videoRenderer.lengthText ? e.videoRenderer.lengthText.simpleText : '') + ' - ' + (e.videoRenderer.viewCountText ? e.videoRenderer.viewCountText.simpleText : '') + '</small>'
                            + '    </div>'
                            + '    <div class="float-right action">'
                            + '         <span class="add-to-playlist" title="Thêm vào hàng đợi" data-id="' + e.videoRenderer.videoId + '" data-url="' + e.videoRenderer.thumbnail.thumbnails[0].url + '" data-title="' + _htmlEncode(e.videoRenderer.title.runs[0].text) + '"><i class="fa fa-indent" aria-hidden="true"></i></span>'
                            + '    </div'
                            + '</div>';
                        $('#list-search').append(st);
                    });
                }
            });
        });

        $("#list-search").on("click", ".add-to-playlist", function (e) {
            e.preventDefault();
            e.stopPropagation();
            var videoId = $(this).data('id');
            var videoTitle = $(this).data('title');
            var videoImage = $(this).data('url');
            var item = { id: videoId, title: videoTitle, image: videoImage };
            if ($(videoLists).filter(function (i, e) { return e.id == videoId }).length == 0) {
                videoLists.push(item);
                storage.setItem("ListVideo", videoLists);
                Core.alert("Đã thêm vào hàng đợi", true);
            } else {
                Core.alert("Video đã tồn tại trong hàng đợi", false);
            }
        });
        $('#playlist-video').on('click', function () {
            $('#playlist-modal').modal('show');
            _loadPlaylistVideo();
        });
    }

    function _loadPlaylistVideo() {
        $('#playlist-content').empty();
        if (videoLists.length == 0) {
            $('#playlist-content').html('<div style="margin: auto">Danh sách rỗng</div>');
            return;
        }
        $(videoLists).each(function (i, e) {
            var item = '<div class="video-item" data-id="' + e.id + '" style="display: flex">'
                + '    <div class="float-left image">        <img src="' + e.image + '" width="100">    </div>'
                + '    <div class="float-left title">        <span>' + e.title + '</span></div>'
                + '</div>';
            $('#playlist-content').append(item);
        });
    }

    function _playVideoById(id) {
        $.get("/Home/GetLinks?url=https://www.youtube.com/watch?v=" + id, function (data, status) {
            if (data.isSuccess) {
                $(data.youtubeVideos).each(function (i, e) {
                    $("#list-videos").append('<span class="btn btn-warning play-video" data-url="' + e.url + '"><i class="fa fa-play" aria-hidden="true"></i> ' + e.resolution + '</span>')
                });
                if (data.youtubeVideos.length > 0) {
                    $("#list-videos").append('<span class="btn btn-success share" data-url="https://www.youtube.com/watch?v=' + id + '"><i class="fa fa-send"></i> Share</span>')
                }
                $("#video-control").html('<video autoplay="autoplay" src="' + data.youtubeVideos[0].url + '" class="col-12" controls="controls"></video>')
                $("#video-title").text(data.title);
                $("#list-videos").get(0).scrollIntoView();
                _createEndedEvent();
            } else {
                alert("Can't get this video!");
            }
            dom.btnGetVideo.removeAttr("disabled");
        });
    }

    function _createEndedEvent() {
        $('video').on('ended', function (e) {
            $("#video-control").html('');
            $("#video-title").text('');
            $("#list-videos").html('');
            if (videoLists.length > 0) {
                _playVideoById(videoLists[0].id);
                videoLists = videoLists.slice(1);
                storage.setItem('ListVideo', videoLists);
            }
        });
    }
    return {
        init: function () {
            _init();
            _bindEvents();
        },
        listVideos: videoLists
    }
}();


$(function () {
    homeIndex.init();
    $('.top-trending').click();
})

$('#list-search').on('scroll', function () {
    let div = $(this).get(0);
    if (div.scrollTop + div.clientHeight >= div.scrollHeight) {
        $.get("/Home/SearchVideos?key=" + $('#list-search').data('key') + "&continuation=" + $('#list-search').data('continuation'), function (data, status) {
            if (data.videos.length == 0) {
                $("#list-search").html('Không tìm thấy');
            } else {
                $('#list-search').attr('data-continuation', data.continuation);
                $(data.videos).each(function (i, e) {
                    var st = '<div class="video-item" data-id="' + e.videoRenderer.videoId + '">'
                        + '    <div class="float-left image">'
                        + '        <img src="' + e.videoRenderer.thumbnail.thumbnails[0].url + '" width="100" />'
                        + '    </div>'
                        + '    <div class="float-left title">'
                        + '        <span>'
                        + e.videoRenderer.title.runs[0].text
                        + '        </span>'
                        + '        <small>Kênh: ' + e.videoRenderer.ownerText.runs[0].text + '</small>'
                        + '        <small>Thời gian: ' + (e.videoRenderer.lengthText ? e.videoRenderer.lengthText.simpleText : '') + ' - ' + (e.videoRenderer.viewCountText ? e.videoRenderer.viewCountText.simpleText : '') + '</small>'
                        + '    </div>'
                        + '    <div class="float-right action">'
                        + '         <span class="add-to-playlist" title="Thêm vào hàng đợi"><i class="fa fa-plus-square" aria-hidden="true"></i></span>'
                        + '    </div'
                        + '</div>';
                    $('#list-search').append(st);
                });
            }
        });
    }
});