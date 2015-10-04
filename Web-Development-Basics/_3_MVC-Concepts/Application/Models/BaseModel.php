<?php

namespace TODO\Application\Models;

use TODO\Framework\Database;

class BaseModel {

    /** @var $database Database */
    protected $databaseInstance;

    public function __construct( $databaseInstance ) {
        $this->databaseInstance = $databaseInstance;
    }
}