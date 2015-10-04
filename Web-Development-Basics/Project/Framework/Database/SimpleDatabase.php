<?php

namespace Framework\Database;

use Framework\App;

class SimpleDatabase {
    protected $connectionAlias = 'default';
    private $_database = null;
    private $_statement = null;
    private $_params = array();
    private $_sql;

    public function __construct($connection = null) {
        if ($connection instanceof \PDO) {
            $this->_database = $connection;
        } else if ($connection != null) {
            $this->_database = App::getInstance()->getDatabaseConnection($connection);
            $this->connectionAlias = $connection;
        } else {
            $this->_database = App::getInstance()->getDatabaseConnection($this->connectionAlias);
        }
    }

    /**
     * @param $sql
     * @param array $params
     * @param array $pdoOptions
     * @return \Framework\Database\SimpleDatabase
     */
    public function prepare($sql, $params = array(), $pdoOptions = array()) {
        $this->_statement = $this->_database->prepare($sql, $pdoOptions);
        $this->_params = $params;
        $this->_sql = $sql;

        return $this;
    }

    /**
     * @param array $params
     * @return \Framework\Database\SimpleDatabase
     */
    public function execute($params = array()) {
        if ($params) {
            $this->_params = $params;
        }

        $this->_statement->execute($this->_params);
        return $this;
    }

    public function fetchAllAssoc() {
        return $this->_statement->fetchAll(\PDO::FETCH_ASSOC);
    }

    public function fetchRowAssoc() {
        return $this->_statement->fetch(\PDO::FETCH_ASSOC);
    }

    public function fetchAllNum() {
        return $this->_statement->fetchAll(\PDO::FETCH_NUM);
    }

    public function fetchRowNum() {
        return $this->_statement->fetch(\PDO::FETCH_NUM);
    }

    public function fetchAllObj() {
        return $this->_statement->fetchAll(\PDO::FETCH_OBJ);
    }

    public function fetchRowObj() {
        return $this->_statement->fetch(\PDO::FETCH_OBJ);
    }

    public function fetchAllColumn($column) {
        return $this->_statement->fetchAll(\PDO::FETCH_COLUMN, $column);
    }

    public function fetchRowColumn($column) {
        return $this->_statement->fetch(\PDO::FETCH_COLUMN, $column);
    }

    public function fetchAllClass($class) {
        return $this->_statement->fetchAll(\PDO::FETCH_CLASS, $class);
    }

    public function fetchRowClass($class) {
        return $this->_statement->fetch(\PDO::FETCH_BOUND, $class);
    }

    public function getLastInsertId() {
        return $this->_database->lastInsertId();
    }

    public function getAffectedRows() {
        return $this->_statement->rowCount();
    }

    public function getStatement() {
        return $this->_statement;
    }
}