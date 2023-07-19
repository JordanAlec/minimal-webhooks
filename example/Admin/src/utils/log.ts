import Pino from 'pino';

const defaultEnvironment = 'development';
const environment = process.env.NODE_ENV || defaultEnvironment;
const isDevelopment = environment === defaultEnvironment || !environment;
const name = isDevelopment ? `${environment?.slice(0, 3)}-${process.env.APP_NAME}` : `${environment?.slice(0, 4)}-${process.env.APP_NAME}`;
const level = process.env.LOG_LEVEL || 'info';

const log = Pino({
  name,
  level,
  timestamp: () => `,"time":"${new Date(Date.now()).toISOString()}"`,
  formatters: {
    level: (label) => {
      return { level: label };
    },
  },
  transport: isDevelopment
    ? {
        target: 'pino-pretty',
        options: {
          colorize: true,
        },
      }
    : undefined,

  mixin: () => ({
     application_name: name,
     env: environment
    }),
});

log.info(
  {
    application_name: name,
    env: environment,
    log_level: level,
  }
);

export default log;
