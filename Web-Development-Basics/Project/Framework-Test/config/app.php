<?php

$configurations['default_controller'] = 'index';
$configurations['default_method'] = 'index2';
$configurations['namespaces']['Controllers'] = '../controllers/';

$configurations['session']['autostart'] = true;
$configurations['session']['type'] = 'native';
$configurations['session']['name'] = '__sess';
$configurations['session']['lifetime'] = 3600;
$configurations['session']['path'] = '/';
$configurations['session']['domain'] = '';
$configurations['session']['secure'] = false;
$configurations['session']['databaseConnection'] = 'default';
$configurations['session']['databaseTable'] = 'sessions';

return $configurations;