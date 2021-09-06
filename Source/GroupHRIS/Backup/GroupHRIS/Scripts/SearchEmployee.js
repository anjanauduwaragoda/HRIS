 
    var popup;
    function SelectName() {
        popup = window.open("search.aspx", "Popup", "width=500,height=450");
        popup.focus();
    }

    function SetName() {
        if (window.opener != null && !window.opener.closed) {
            var txtName = window.opener.document.getElementById("txtName");
            txtName.value = document.getElementById("txtdescription").value;
        }
        window.close();
    }

 