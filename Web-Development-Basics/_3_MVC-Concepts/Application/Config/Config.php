<?php

namespace TODO\Application\Config;

class Config {

    const DB_INSTANCE_NAME = 'todo_list';
    const DB_DRIVER = 'mysql';
    const DB_USERNAME = 'root';
    const DB_PASSWORD = 'sera';
    const DB_NAME = 'todo_list';
    const DB_HOST = 'localhost';

    const VENDOR_NAMESPACE = 'TODO\\';

    const APPLICATION_NAMESPACE = 'Application\\';
    const FRAMEWORK_NAMESPACE = 'Framework\\';
    const CONTROLLERS_NAMESPACE = 'Controllers\\';

    const CONTROLLERS_FOLDER = 'Application\Controllers\*.php';
    const VIEW_FOLDER = 'Application\Views';

    const CONTROLLERS_SUFFIX = 'Controller';
    const PHP_EXTENSION = '.php';

    const DEFAULT_CONTROLLER = 'users';
    const DEFAULT_ACTION = 'login';

}