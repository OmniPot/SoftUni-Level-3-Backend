<?php

class Db {
    private static $_instance = null;

    private function __construct(){ }

    /*
     * @return Db
     */
    public static function getInstance(){
        if (self::$_instance == null) {
            self::$_instance = new PDO(
                'mysql:host=localhost;dbname=localization;charset=utf8;"',
                'root',
                'sera'
            );
        }

        return self::$_instance;
    }
}