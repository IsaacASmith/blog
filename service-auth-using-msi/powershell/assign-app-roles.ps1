$tenantId = '5ed4c33c-cdb7-4829-9b56-7a8f1f23691a'

$webApiEnterpriseAppObjectId = '0613041d-531c-48ab-80a8-9e633c911f05'

#Services
$functionObjectId = 'f81d8921-d4d8-4230-9062-e377a3ff12f4'

#Roles
$dataReaderRoleId = 'ebb58188-f7f0-484b-be71-de6bcc5ba8e2'
$dataWriterRoleId = 'dbffc013-f721-41f5-bc7c-9eaf879e8f5f'

Connect-AzureAD -TenantId $tenantId

#Grant data reader role to the az function
New-AzureADServiceAppRoleAssignment -ObjectId $functionObjectId `
                                    -PrincipalId $functionObjectId `
                                    -ResourceId $webApiEnterpriseAppObjectId `
                                    -Id $dataReaderRoleId

#Grant data writer role to the az function
New-AzureADServiceAppRoleAssignment -ObjectId $functionObjectId `
                                    -PrincipalId $functionObjectId `
                                    -ResourceId $webApiEnterpriseAppObjectId `
                                    -Id $dataWriterRoleId
