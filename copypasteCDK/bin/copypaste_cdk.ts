#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from '@aws-cdk/core';
import { CopypasteCdkStack } from '../lib/copypaste_cdk-stack';

const app = new cdk.App();
new CopypasteCdkStack(app, 'CopypasteCdkStack');
