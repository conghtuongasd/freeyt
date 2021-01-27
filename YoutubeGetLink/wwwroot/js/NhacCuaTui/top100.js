var top100 = function () {
    var dom = {

    }

    function _initEvent() {
        $('.top-song-image, .top-song-title').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                url: '/NhacCuaTui/GetLink',
                dataType: 'json',
                type: 'POST',
                data: { link: $(this).closest('.top-song-item').data('link') },
                success: function (result) {
                    if (result.success) {
                        $('#audio-control').attr("src", result.data.linkMp3);
                        $('#audio-thumb').attr("src", result.data.image);
                        $('#audio-name').text(result.data.name);
                        $('#audio-singer').text(result.data.singer);
                        $('.audio-background').show();
                    }
                },
                error: function () {
                    alert("loi");
                }
            });
        })
    }

    return {
        init: function () {
            _initEvent();
        }
    }
}();
top100.init();