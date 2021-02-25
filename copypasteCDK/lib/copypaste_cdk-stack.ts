import * as cdk from '@aws-cdk/core';
import * as cognito from '@aws-cdk/aws-cognito';
import * as iam from '@aws-cdk/aws-iam';

export class CopypasteCdkStack extends cdk.Stack {
  constructor(scope: cdk.Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props)

    //Prefix resources with this name
    let prefix:string = "CopyPasteWorld";


    // cognito identity pool  -- Unity project uses the id for this pool.
    const identityPool = new cognito.CfnIdentityPool(this, `${prefix}Pool`, {
      allowUnauthenticatedIdentities: true,
      identityPoolName: `${prefix}IdPool`,
    });

    

    // iam role with policy for PutEvents and UpdateEndpoint
    const unauthRole = new iam.Role(this, `${prefix}UynauthRole`, {
      roleName: `${prefix}cognitoUnauthRole`,
      assumedBy: new iam.FederatedPrincipal('cognito-identity.amazonaws.com', {
        "StringEquals": { "cognito-identity.amazonaws.com:aud": identityPool.ref },
        "ForAnyValue:StringLike": { "cognito-identity.amazonaws.com:amr": "unauthenticated" },
      }, "sts:AssumeRoleWithWebIdentity"),
    });

  
    //connect cognito unauth to iam role
    const defaultPolicy = new cognito.CfnIdentityPoolRoleAttachment(this, 'DefaultValid', {
      identityPoolId: identityPool.ref,
      roles: {
          'unauthenticated': unauthRole.roleArn,
      }
    });

    // add Rekognition DetectText policy to iam role
    unauthRole.addToPolicy( new iam.PolicyStatement({
      effect: iam.Effect.ALLOW,
      actions: [
        "rekognition:DetectText",
      ],
      resources: ["*"], 
    }));  


  }
}
