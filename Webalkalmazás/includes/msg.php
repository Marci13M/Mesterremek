<div class="msg-container" id="msg-container">
    <div class="msg error" id="msg">
        <p id="msg-text"></p>
    </div>
</div>

<script>
    var showTime = 3000;
    var waitTime = 1000;
    var messageShowing = false;

    function ShowMessage(){
        if(messageShowing){
            return;
        }

        if(sessionStorage.getItem("messages")){
            messages = JSON.parse(sessionStorage.getItem("messages")); // üzenetek
        } else {
            messages = [];
        }
        
        if(messages.length > 0){
            // üzenet felöltése
            $("#msg").removeClass("error");
            $("#msg").removeClass("warning");
            $("#msg").removeClass("success");
            
            $("#msg").addClass(messages[0][0]);
            $("#msg-text").text(messages[0][1]);
            
            // üzenet megjelenítése
            messageShowing = true;
            $("#msg-container").fadeIn(150);
            setTimeout(() => { // üzenet eltávolítása
                $("#msg-container").fadeOut(150);
                // üzenet törlése a listából
                messages.shift(-1);
                sessionStorage.setItem("messages", JSON.stringify(messages));
                messageShowing = false;
                
                // hogyha még van üzenet megjelenítjük
                if(messages.length > 0){
                    setTimeout(() => {
                        ShowMessage();
                    }, waitTime);
                }
            }, showTime);   
        }
    }

    function AddMessage(type, message){
        if(sessionStorage.getItem("messages") !== null){
            messages = JSON.parse(sessionStorage.getItem("messages"));
            messages.push([type, message]);
            sessionStorage.setItem("messages", JSON.stringify(messages));
        } else {
            sessionStorage.setItem("messages", JSON.stringify([[type, message]]));
        }
        ShowMessage();
    }

    ShowMessage();
</script>

<?php

    if(isset($_GET["error"])){
        AddJavascriptMessage("error", htmlspecialchars($_GET["error"]));
    }

    if(isset($_GET["success"])){
        AddJavascriptMessage("success", htmlspecialchars($_GET["success"]));
    }

    if(isset($_GET["warning"])){
        AddJavascriptMessage("warning", htmlspecialchars($_GET["warning"]));
    }

    function AddJavascriptMessage($type, $message){ ?>
        <script>
            AddMessage("<?php echo($type); ?>", "<?php echo($message); ?>");
                const url = new URL(window.location.href);
                
                // URL paraméter törlése
                url.searchParams.delete("<?php echo($type); ?>");

                // Oldal újratöltése
                window.history.replaceState(null, "", url.toString());
        </script>
    <?php }

?>