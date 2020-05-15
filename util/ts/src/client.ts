// This file is auto-generated, don't edit it
/**
 * For viapi
 */
import { Readable } from 'stream';
import url from 'url';
import path from 'path';
import https from 'https';
import fs from 'fs';
import { promisify } from 'util';
import * as $tea from '@alicloud/tea-typescript';



export default class Client {

  /**
   * Judge whether origin starts with sub, if yes, return true, or return false
   * @param origin the original string
   * @param sub the prefix string
   * @return the judge result
   */
  static startsWith(origin: string, sub: string): boolean {
    return origin.startsWith(sub);
  }

  /**
   * If whether origin contains regrex, return string, or return ''
   * @param origin the original string
   * @param regrex the regular expression
   * @return the matched string
   */
  static match(origin: string, regrex: string): string {
    return origin.match(regrex) ? origin : '';
  }

  /**
   * Decode origin with decodeType
   * @param origin the original string
   * @param decodeType decode type
   * @return the decoded string
   */
  static decode(origin: string, decodeType: string): string {
    return decodeURIComponent(origin);
  }

  /**
   * Get file name from url
   * @param urlStr the url
   * @return the file name
   */
  static getNameFromUrl(urlStr: string): string {
    return url.parse(urlStr).path || '';
  }

  /**
   * Get file name from  path
   * @param pathStr the path
   * @return the file name
   */
  static getNameFromPath(pathStr: string): string {
    return path.basename(pathStr);
  }

  /**
   * Intercepts the last string according to sub
   * @param path the original string
   * @param sub the cutting string
   * @return the last string
   */
  static subStringAfterLast(path: string, sub: string): string {
    const strArr = path.split(sub);
    return strArr.pop();
  }

  /**
   * Get stream from net
   * @param url the file url
   * @return the stream
   */
  static async getStreamFromNet(url: string): Promise<Readable> {
    return new Promise((resolve, reject) => {
      https.get(url, (res) => {
        resolve(res);
      })
    })
  }

  /**
   * Get stream from path
   * @param filepath the file path
   * @return the stream
   */
  static async getStreamFromPath(filepath: string): Promise<Readable> {
    const readFile = promisify(fs.readFile); 
    const fileData = await readFile(filepath,'utf8')
    return Readable.from(fileData);
  }

  /**
   * Close the body
   * @param body the stream that needs to be closed
   */
  static close(body: Readable): void {
    body.destroy();
  }

}
