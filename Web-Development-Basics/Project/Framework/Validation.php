<?php

namespace Framework;

class Validation {
    private $_rules = array();
    private $_errors = array();

    public function setRule($rule, $value, $params = null, $name = null) {
        $this->_rules[] = array('val' => $value, 'rule' => $rule, 'par' => $params, 'name' => $name);
        return $this;
    }

    public function validate() {
        $this->_errors = array();
        if (count($this->_rules) > 0) {
            foreach ($this->_rules as $rule) {
                if (!$this->$rule['rule']($rule['val'], $rule['par'])) {
                    if ($rule['name']) {
                        $this->_errors[] = $rule['name'];
                    } else {
                        $this->_errors[] = $rule['rule'];
                    }
                }
            }
        }

        return (bool)!count($this->_errors);
    }

    public function __call($firstValue, $secondValue) {
        throw new \Exception('Invalid validation rule', 500);
    }

    public function getErrors() {
        return $this->_errors;
    }

    public static function matches($firstValue, $secondValue) {
        return $firstValue == $secondValue;
    }

    public static function minLength($value, $minLength) {
        return (mb_strlen($value) >= $minLength);
    }

    public function custom($firstValue, $secondValue) {
        if ($secondValue instanceof \Closure) {
            return (boolean)call_user_func($secondValue, $firstValue);
        } else {
            throw new \Exception('Invalid validation function', 500);
        }
    }
}