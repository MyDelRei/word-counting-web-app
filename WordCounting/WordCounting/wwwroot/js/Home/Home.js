$(document).ready(function () {
    let btnFile = $(".file-input")
    let txtArea = $("#txtBox")
    let btnCount = $('#btn-count')
    

    btnFile.change(function () {
        let file = this.files[0]; // Get the selected file
        if (file) {
            let formData = new FormData();
            formData.append("UploadFile", file);
            console.log(formData)
            // Must match property name in UploadModel

            $.ajax({
                url: "/Home/UploadFile",
                type: "POST",
                data: formData,
                cache: false, 
                contentType: false,
                processData: false,
                success: function (response) {
                    console.log("Server Response:", response); // debugging
                    if (response.success) {
                        $(".textarea").val(response.content);
                    } else {
                        alert("Error: " + response.Message);
                    }
                },
                error: function (xhr) {
                    console.log("AJAX Error:", xhr.responseText);
                }
            });


        } else {
            txtArea.val(""); // Clear textarea if no file is selected
        }
    })

    btnCount.on('click', () => {
        let content = txtArea.val()
        txtArea.val("")
        

        $.ajax({
            url: `/Home/Counting?word=${encodeURIComponent(content)}`,
            type: 'GET',
            success: function (response) {
                if (response.repeatword && response.number_of_word === null) {
                    return;
                }
                let result = "";
                Object.entries(response.repeatword).forEach(([key, value]) => {
                    result += `${key} = ${value}\n`;
                })
                console.log(result);
                txtArea.val(result);

                $('#wordCount').text('word: '+response.number_of_word)
            }
        })

    })

})