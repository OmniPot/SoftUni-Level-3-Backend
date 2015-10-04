<?php

require_once 'translations.php' ;

?>
<html>
    <head>
        <meta charset="utf-8">
        <title>Database-Exercise</title>
    </head>
    <body>
    <header>
        <a href="?lang=bg">BG</a> | <a href="?lang=en">EN</a>

        <h1>
            <?= __("greeting_header_hello"); ?>
        </h1>
    </header>
    <div>
        <p>
            <?= __("welcome_message"); ?>
        </p>
    </div>
    <a href="editTranslations.php">Edit</a>
    </body>
</html>

