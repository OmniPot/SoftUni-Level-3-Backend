<?php

require_once 'Localization.php' ;
require_once 'Db.php';

if (isset($_GET['lang'])) {
    $lang = $_GET['lang'];

    $db = Db::getInstance();
    $res = $db->query('show columns from translations');
    $columns = $res->fetchAll(PDO::FETCH_ASSOC);
    $possibleLanguages = array();

    foreach ($columns as $column) {
        if (strpos($column['Field'], 'text_') === 0) {
            $language = substr($column['Field'], 5, strlen($column['Field'] - 5));
            array_push($possibleLanguages, $language);
        }
    }

    if (!in_array($lang, $possibleLanguages)) {
        throw new Exception('Invalid language');
    }

    setcookie('lang', $lang);
    $_COOKIE['lang'] = $lang;

    Localization::$LANG_DEFAULT = $possibleLanguages[0];
}

function __($tag)
{
    $lang = isset($_COOKIE['lang'])
        ? $_COOKIE['lang']
        : Localization::$LANG_DEFAULT;


    $query = Db::getInstance()->query("
        SELECT
          text_{$lang}
        FROM
          translations
        WHERE
          tag = '$tag';
    ");

    $row = $query->fetch(PDO::FETCH_NUM);
    return $row[0];
}