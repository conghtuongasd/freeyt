var nhacCuaTui = function () {
    var dom = {

    };

    function _initControl() {
        Core.loadPartial('#top-100', '/NhacCuaTui/Top100')

        $("#search-song").kendoAutoComplete({
            dataTextField: "link",
            filter: "contains",
            minLength: 2,
            placeholder: 'Tìm kiếm',
            dataSource: {
                type: "json",
                serverFiltering: true,
                transport: {
                    read: function (options) {
                        $.ajax({
                            url: '/NhacCuaTui/Search',
                            contentType: 'application/json',
                            dataType: 'json',
                            type: 'POST',
                            data: JSON.stringify(options.data),
                            success: function (result) {
                                options.success(result);
                            }
                        });
                    }
                }
            },
            dataTextField: "name",
            headerTemplate: '<div class="dropdown-header k-widget k-header">' +
                '<span>Tìm kiếm bài hát</span>' +
                '</div>',
            footerTemplate: '#: instance.dataSource.total() # Bài',
            template: '<div class="song-item">' +
                '<img class="thumb" src="#:data.thumb#" height="60"/></span>' +
                '<div class="song-tile">' +
                '<span class="k-state-default song-info"><b>#: data.name #</b><p style="margin-left: 5px">Trình bày: #:data.singer#</p></span>' +
                '</div>'+
                '</div>',
            select: function (e) {
                $.ajax({
                    url: '/NhacCuaTui/GetLink',
                    dataType: 'json',
                    type: 'POST',
                    data: { link: e.dataItem.link },
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
            }
        });
    }

    function _initEvent() {

    }

    return {
        init: function () {
            _initControl();
            _initEvent();
        }
    }
}();
nhacCuaTui.init();