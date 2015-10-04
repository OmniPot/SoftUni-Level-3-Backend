<?php

namespace Framework;

final class Loader {
    private static $namespaces = array();

    private function __construct() {

    }

    public static function getNamespaces() {
        return self::$namespaces;
    }

    public static function removeNamespace($namespace) {
        unset(self::$namespaces[$namespace]);
    }

    public static function clearNamespaces() {
        self::$namespaces = array();
    }

    public static function registerAutoLoad() {
        spl_autoload_register(array('\Framework\Loader', 'autoload'));
    }

    public static function autoload($class) {
        self::loadClass($class);
    }

    public static function loadClass($class) {
        foreach (self::$namespaces as $namespace => $value) {
            if (strpos($class, $namespace) === 0) {
                $file = str_replace('\\', DIRECTORY_SEPARATOR, $class);
                $file = substr_replace($file, $value, 0, strlen($namespace)) . '.php';
                $file = realpath($file);
                if ($file && is_readable($file)) {
                    include $file;
                } else {
                    throw new \Exception('File cannot be included' . $file);
                }

                break;
            }
        }
    }

    public static function registerNamespaces($namespaces) {
        if (is_array($namespaces)) {
            foreach ($namespaces as $namespace => $value) {
                self::registerNamespace($namespace, $value);
            }
        } else {
            throw new \Exception('Invalid namespace');
        }
    }

    public static function registerNamespace($namespace, $path) {
        $namespace = trim($namespace);
        if (strlen($namespace) > 0) {
            if (!$path) {
                throw new \Exception('Invalid path');
            }

            $_path = realpath($path);
            if ($_path && is_dir($_path) && is_readable($_path)) {
                self::$namespaces[$namespace . '\\'] = $path . DIRECTORY_SEPARATOR;
            } else {
                throw new \Exception('Namespace directory read error:' . $path);
            }
        } else {
            throw new \Exception('Invalid namespace:' . $namespace);
        }
    }
}