<?php

namespace Framework\Sessions;

use Framework\Database\SimpleDatabase;

class DatabaseSession extends SimpleDatabase implements ISession {
    private $sessionName;
    private $tableName;
    private $lifeTime;
    private $path;
    private $domain;
    private $secure;
    private $sessionId = null;
    private $sessionData = array();

    public function __construct(
        $databaseConnection,
        $sessionName,
        $tableName = 'sessions',
        $lifeTime = 3600,
        $path = null,
        $domain = null,
        $secure = false
    ) {
        parent::__construct($databaseConnection);
        $this->sessionName = $sessionName;
        $this->tableName = $tableName;
        $this->lifeTime = $lifeTime;
        $this->path = $path;
        $this->domain = $domain;
        $this->secure = $secure;
        $this->sessionId = $_COOKIE[$sessionName];

        if (rand(0, 50) == 1) {
            $this->_clearExpiredSessions();
        }

        if (strlen($this->sessionId) < 32) {
            $this->_startNewSession();
        } else if (!$this->_validateSession()) {
            $this->_startNewSession();
        }
    }

    public function __get($name) {
        return $this->sessionData[$name];
    }

    public function __set($name, $value) {
        $this->sessionData[$name] = $value;
    }

    public function getSessionId() {
        return $this->sessionId;
    }

    public function saveSession() {
        if ($this->sessionId) {
            $query = 'UPDATE ' . $this->tableName . ' SET data = ?, valid_until = ? WHERE id = ?';
            $sessionSaveData = array(serialize($this->sessionData), (time() + $this->lifeTime), $this->sessionId);
            $this->prepare($query, $sessionSaveData)->execute();

            setcookie(
                $this->sessionName,
                $this->sessionId,
                (time() + $this->lifeTime),
                $this->path,
                $this->domain,
                $this->secure,
                true
            );
        }
    }

    public function destroySession() {
        if ($this->sessionId) {
            $query = 'DELETE FROM ' . $this->tableName . 'WHERE id = ?';
            $sessionDestroyData = array($this->sessionId);
            $this->prepare($query, $sessionDestroyData)->execute();
        }
    }

    private function _startNewSession() {
        $this->sessionId = md5(uniqid('Framework', true));

        $query = 'INSERT INTO ' . $this->tableName . ' (id,valid_until) VALUES(?,?)';
        $newSessionData = array($this->sessionId, (time() + $this->lifeTime));
        $this->prepare($query, $newSessionData)->execute();

        setcookie(
            $this->sessionName,
            $this->sessionId,
            (time() + $this->lifeTime),
            $this->path,
            $this->domain,
            $this->secure,
            true
        );
    }

    private function _validateSession() {
        if ($this->sessionId) {
            $query = 'SELECT * FROM ' . $this->tableName . ' WHERE id = ? AND valid_until <= ?';
            $sessionValidationsData = array($this->sessionId, (time() + $this->lifeTime));
            $sessionData = $this->prepare($query, $sessionValidationsData)->execute()->fetchAllAssoc();

            if (is_array($sessionData) && count($sessionData) == 1 && $sessionData[0]) {
                $this->sessionData = unserialize($sessionData[0]['data']);
                return true;
            }
        }

        return false;
    }

    private function _clearExpiredSessions() {
        $query = 'DELETE FROM ' . $this->tableName . ' WHERE valid_until <= ?';
        $invalidSessionsData = array(time());
        $this->prepare($query, $invalidSessionsData)->execute();
    }
}