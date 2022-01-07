export const CLIENT_STATUSES = {
  starting: "Starting",
  ready: "Ready",
  testing: "Testing",
  terminating: "Terminating",
};

export const CLIENT = {
  clientStatusId: "clientStatusId",
  entityType: "entityType",
  lastStatusChange: "lastStatusChange",
  lastUpdated: "lastUpdated",
  loadClientId: "loadClientId",
  message: "message",
  name: "name",
  prometheus: "prometheus",
  region: "region",
  startTime: "startTime",
  startupArgs: "startupArgs",
  status: "status",
  tag: "tag",
  version: "version",
  zone: "zone",
};

export const CONFIG = {
  baseUrl: "baseUrl",
  delayStart: "delayStart",
  dryRun: "dryRun",
  duration: "duration",
  entityType: "entityType",
  files: "files",
  id: "id",
  name: "name",
  maxErrors: "maxErrors",
  strictJson: "strictJson",
  verbose: "verbose",
  verboseErrors: "verboseErrors",
  randomize: "randomize",
  runLoop: "runLoop",
  servers: "server",
  sleep: "sleep",
  tag: "tag",
  timeout: "timeout",
};

export const TEST_RUN = {
  entityType: "entityType",
  id: "id",
  name: "name",
  config: "loadTestConfig",
  clients: "loadClients",
  createdTime: "createdTime",
  scheduledStartTime: "startTime",
  totalCompletionTime: "completedTime",
  results: "clientResults",
};

export const RESULT = {
  client: "loadClient",
  requestCount: "totalRequests",
  successfulRequestCount: "successfulRequests",
  failedRequestCount: "failedRequests",
};
