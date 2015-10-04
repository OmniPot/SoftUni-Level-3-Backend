<?php

require_once 'Db.php';
require_once 'Localization.php';

Localization::$LANG_DEFAULT = $_COOKIE['lang'];

$db = Db::getInstance();
$resTranslations = $db->query("SELECT id, tag, text_en, text_bg FROM TRANSLATIONS");
$translations = $resTranslations->fetchAll(PDO::FETCH_ASSOC);

?>

    <html>
    <head>
        <meta charset="UTF-8">
        <title>Edit</title>
    </head>
    <body>
    <form method="post">
        <?php foreach ($translations as $translation) : ?>
            <div class="source-translation"><?= $translation['text_' . Localization::$LANG_DEFAULT]; ?> </div>
            <textarea name="<?= $translation['id']; ?>"><?= $translation['text_bg']; ?></textarea>
        <?php endforeach; ?>
        <input type="submit" name="submit">
    </form>
    </body>
    </html>

<?php

if (isset($_POST['submit'])) {
    foreach ($translations as $translation) {
        $id = $translation['id'];
        if ($translation['text_bg'] != $_POST[$id]) {
            $db = Db::getInstance();
            $updateQuery = "UPDATE TRANSLATIONS SET text_bg = ? WHERE id = ?";

            $statement = $db->prepare($updateQuery);
            $statement->bindParam(1, $_POST[$id], PDO::PARAM_STR);
            $statement->bindParam(2, $id, PDO::PARAM_INT);
            $statement->execute();
        }
    }

    header('Location: editTranslations.php');
}