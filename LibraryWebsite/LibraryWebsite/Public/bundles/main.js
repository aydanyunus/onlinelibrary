$(function () {

    $(".getBook").click(function () {

        var id = $(this).attr("data-id");
      
        $.ajax({
            url: "http://localhost:51967/Ajax/GetBook/",
            type: "post",
            data: {id:id},
            datatype: "json",
            success: function (res) {
                if (res.status == 200) {
                    
                  alert("kitab ugurla elave olundu");

                } else {
                    alert("5-den artiq kitab olmaz");
                }
            
            
            
               
                                                 
                              
            }
            
        })

    })


    $(".fa-close").click(function () {
        var id = $(this).attr("data-id");
        
        $.ajax({
            url: "http://localhost:51967/Ajax/Close/",
            type: "post",
            data: { id: id },
            datatype: "json",
            success: function (res) {
                alert("kitab silindi.");                        
                refresh();

               
            }
        })
    })

    function refresh() {
        setTimeout(function () {
            location.reload()
        }, 200);
    }



    
});