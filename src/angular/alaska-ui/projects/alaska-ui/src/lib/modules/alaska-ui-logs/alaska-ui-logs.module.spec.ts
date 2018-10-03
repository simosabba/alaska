import { AlaskaUiLogsModule } from './alaska-ui-logs.module';

describe('AlaskaUiLogsModule', () => {
  let alaskaUiLogsModule: AlaskaUiLogsModule;

  beforeEach(() => {
    alaskaUiLogsModule = new AlaskaUiLogsModule();
  });

  it('should create an instance', () => {
    expect(alaskaUiLogsModule).toBeTruthy();
  });
});
