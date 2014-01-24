var Gateway = Gateway || {};
Gateway.Api = (function ($) {
    return {
        initialize: function () {
            var temperature = Gateway.Common.getParameterByName("temperature");
            var vibration = Gateway.Common.getParameterByName("vibration");
            var orientation = Gateway.Common.getParameterByName("orientation");
            var wetdry = Gateway.Common.getParameterByName("wetdry");

            if(temperature > 64)
            {
                Gateway.Azure.postToAzure(temperature);
            }
        }
    };

}(jQuery));



Gateway.Azure = (function ($) {
    var azureClient = null;
    var enableLogging = false;

    return {
        getTable: function (tableName) {
            if (!azureClient)
                azureClient = new WindowsAzure.MobileServiceClient('https://aiemobileservice.azure-mobile.net/', 'NYuUVUztAwEXJQZxOFbppximTExpoh26');
            return azureClient.getTable(tableName);
        },

        postToAzure: function (temp) {
            var headers = {
                'X-ZUMO-APPLICATION': 'NYuUVUztAwEXJQZxOFbppximTExpoh26'
            };
            //{ insuredId: "1", equipmentId: "3", temperature: 51, incidentTimestamp: "2013-10-08T12:11:00.000Z", activeIndicator: false }

            $.ajax({
                url: "https://aiemobileservice.azure-mobile.net/tables/equipment_incident",
                type: "POST",
                async: false,
                headers: headers,
                data: { insuredId: '1', equipmentId: '2', temperature: temp },
                dataType: "json",
                success: function (result) {
                    Gateway.Common.log("Azure POST succesful");
                },
                error: function (result) {
                    Gateway.Common.log(("Error:" + result));
                }
            });
        }
    };

}(jQuery));



Gateway.Common = (function ($) {
    return {
        getParameterByName: function (name) {
            name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        },
        log: function (message) {
            $('#errorlog').append($('<li>').text(message));
        }
    };

}(jQuery));