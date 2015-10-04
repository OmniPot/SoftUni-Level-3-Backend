<?php
namespace Core\Drivers;

class MySQLDriver extends DriverAbstract {

    public function __construct($user, $password, $databaseName, $host = null){
        parent::__construct($user, $password, $databaseName, $host);
    }

    /**
     * @return string
     */
    public function getDsn() {
        $dsn = 'mysql:host=' . $this->host . ';dbname=' . $this->databaseName;

        return $dsn;
    }
}