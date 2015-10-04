<?php

$configurations['admin']['namespace'] = 'Controller\Admin1';

$configurations['administration']['namespace'] = 'Controllers\Admin';
$configurations['administration']['controllers']['index']['to'] = 'test';
$configurations['administration']['controllers']['index']['methods']['new'] = '_new';
$configurations['administration']['controllers']['new']['to'] = 'create';

$configurations['*']['namespace'] = 'Controllers';

return $configurations;